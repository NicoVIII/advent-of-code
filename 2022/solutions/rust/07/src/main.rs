use std::{collections::HashMap, env, fs, str::Lines};

#[derive(Debug)]
enum Command {
    CD(String),
    LS,
}

#[derive(Debug)]
enum Output {
    Dir(String),
    Size(usize, String),
}

#[derive(Debug)]
enum Line {
    Command(Command),
    Output(Output),
}

fn read_input() -> String {
    let args: Vec<String> = env::args().collect();
    let filename = &args[1];
    fs::read_to_string(filename).expect("Should have been able to read the file")
}

fn parse_input(lines: Lines<'_>) -> Vec<Line> {
    fn parse_line(line: &str) -> Line {
        if let Some(command) = line.strip_prefix("$ ") {
            if let Some(dir) = command.strip_prefix("cd ") {
                Line::Command(Command::CD(dir.to_string()))
            } else if command == "ls" {
                Line::Command(Command::LS)
            } else {
                panic!("Unknown command: {}", line)
            }
        } else if let Some(dir) = line.strip_prefix("dir ") {
            let dir = dir.to_string();
            Line::Output(Output::Dir(dir))
        } else {
            let mut iter = line.split_whitespace();
            let size = iter
                .next()
                .unwrap()
                .parse()
                .expect("Should have been able to parse size");
            let name = iter.next().unwrap().to_string();
            Line::Output(Output::Size(size, name))
        }
    }
    lines.filter(|x| !x.is_empty()).map(parse_line).collect()
}

#[derive(Debug, Clone)]
struct File {
    name: String,
    size: usize,
}

fn collect_data(lines: &[Line]) -> (HashMap<String, Vec<String>>, HashMap<String, Vec<File>>) {
    let mut subfolder_map: HashMap<String, Vec<String>> = HashMap::new();
    let mut file_map: HashMap<String, Vec<File>> = HashMap::new();
    let mut current_dir = "/".to_string(); // We start in root
    let mut ls = false; // We check, if the output makes sense
    for line in lines {
        match line {
            Line::Command(Command::CD(dir)) => {
                ls = false;
                current_dir = match &dir[..] {
                    "/" => "/".to_string(),
                    ".." => {
                        let last_part = current_dir[..current_dir.len() - 1].rfind('/').unwrap();
                        current_dir[..last_part + 1].to_string()
                    }
                    dir => {
                        format!("{}{}/", current_dir, dir)
                    }
                };
            }
            Line::Command(Command::LS) => ls = true,
            Line::Output(Output::Dir(dir)) => {
                if !ls {
                    panic!("Unexpected ls output");
                }
                match subfolder_map.get_mut(&current_dir) {
                    Some(subfolders) => subfolders.push(dir.to_string()),
                    None => {
                        subfolder_map.insert(current_dir.to_string(), vec![dir.to_string()]);
                    }
                }
            }
            Line::Output(Output::Size(size, name)) => {
                if !ls {
                    panic!("Unexpected ls output");
                }
                let file = File {
                    name: name.to_string(),
                    size: *size,
                };
                match file_map.get_mut(&current_dir) {
                    Some(files) => files.push(file),
                    None => {
                        file_map.insert(current_dir.to_string(), vec![file]);
                    }
                }
            }
        }
    }
    (subfolder_map, file_map)
}

#[derive(Debug)]
struct Folder {
    files: Vec<File>,
    name: String,
    subfolders: Vec<Folder>,
}

fn structure_data(
    path: &str,
    subfolder_map: &HashMap<String, Vec<String>>,
    file_map: &HashMap<String, Vec<File>>,
) -> Folder {
    let name = match path[..path.len() - 1].rfind('/') {
        Some(first_part) => path[first_part + 1..path.len() - 1].to_string(),
        None => "/".to_string(),
    };

    let subfolders = subfolder_map
        .get(path)
        .unwrap_or(&vec![])
        .iter()
        .map(|x| structure_data(&format!("{}{}/", path, x), subfolder_map, file_map))
        .collect();

    Folder {
        files: file_map.get(path).unwrap_or(&vec![]).to_vec(),
        name,
        subfolders,
    }
}

fn prepare_data(input: Vec<Line>) -> Folder {
    let (subfolder_map, file_map) = collect_data(&input);
    structure_data("/", &subfolder_map, &file_map)
}

#[derive(Debug)]
struct FolderWithFullData {
    files: Vec<File>,
    name: String,
    subfolders: Vec<FolderWithFullData>,
    size: usize,
}

fn analyse_data(folder: &Folder) -> FolderWithFullData {
    let mut folder = FolderWithFullData {
        files: folder.files.clone(),
        name: folder.name.clone(),
        subfolders: folder.subfolders.iter().map(analyse_data).collect(),
        size: 0,
    };
    folder.size = folder.files.iter().map(|x| x.size).sum();
    folder.size += folder.subfolders.iter().map(|x| x.size).sum::<usize>();
    folder
}

fn sum_with_max(folder: &FolderWithFullData, max: usize) -> usize {
    let subfolder_sum = folder
        .subfolders
        .iter()
        .map(|folder| sum_with_max(folder, max))
        .sum();

    if folder.size <= max {
        subfolder_sum + folder.size
    } else {
        subfolder_sum
    }
}

fn print_folder(folder: &FolderWithFullData) {
    fn _print_folder(folder: &FolderWithFullData, indent: usize) {
        let indent_step = 3;
        println!("{}|- {} ({})", " ".repeat(indent), folder.name, folder.size);
        for file in &folder.files {
            println!(
                "{}|- {} ({})",
                " ".repeat(indent + indent_step),
                file.name,
                file.size
            );
        }
        for folder in &folder.subfolders {
            _print_folder(folder, indent + indent_step);
        }
    }
    _print_folder(folder, 0);
}

fn get_folder_sizes(folder: &FolderWithFullData) -> Vec<usize> {
    let mut subfolder_sizes: Vec<usize> = folder
        .subfolders
        .iter()
        .flat_map(get_folder_sizes)
        .collect();
    subfolder_sizes.push(folder.size);
    subfolder_sizes
}

fn main() {
    let input = read_input();
    let input = parse_input(input.lines());
    let folder = prepare_data(input);
    let folder = analyse_data(&folder);
    //print_folder(&folder);

    // Part 1
    println!("Part 1: {}", sum_with_max(&folder, 100000));

    // Part 2
    let free_space_needed = 30_000_000 - (70_000_000 - folder.size);
    let sizes = get_folder_sizes(&folder);
    let to_delete = sizes
        .iter()
        .filter(|x| **x > free_space_needed)
        .min()
        .unwrap();
    println!("Part 2: {}", to_delete);
}

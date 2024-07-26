use array_tool::vec::Intersect;
use std::{env, fs, str::Lines};

#[derive(Debug)]
struct Backpack {
    compartment1: String,
    compartment2: String,
}

fn read_input() -> String {
    let args: Vec<String> = env::args().collect();
    let filename = &args[1];
    fs::read_to_string(filename).expect("Should have been able to read the file")
}

fn parse_input(input: Lines<'_>) -> Vec<Backpack> {
    fn parse_line(line: String) -> Option<Backpack> {
        let trimmed = line.trim();
        if !trimmed.is_empty() {
            let length = trimmed.len();
            Some(Backpack {
                compartment1: trimmed[0..length / 2].to_string(),
                compartment2: trimmed[length / 2..length].to_string(),
            })
        } else {
            None
        }
    }

    input
        .filter_map(|line| parse_line(line.to_string()))
        .collect()
}

#[derive(Debug)]
struct Group {
    elf1: String,
    elf2: String,
    elf3: String,
}

fn parse_input2(input: Lines<'_>) -> Vec<Group> {
    let mut chunk = Vec::new();
    let mut groups = Vec::new();
    for line in input {
        if !line.is_empty() {
            chunk.push(line);
            if chunk.len() == 3 {
                groups.push(Group {
                    elf1: chunk[0].to_string(),
                    elf2: chunk[1].to_string(),
                    elf3: chunk[2].to_string(),
                });
                chunk.clear();
            }
        }
    }
    if !chunk.is_empty() {
        panic!("Linenumber was not a multiple of 3");
    }
    groups
}

fn get_shared_items(backpack: &Backpack) -> char {
    let letters1: Vec<char> = backpack.compartment1.chars().collect();
    let letters2: Vec<char> = backpack.compartment2.chars().collect();
    match letters1.intersect(letters2)[..] {
        [letter] => letter,
        [] => panic!("No shared letters"),
        _ => panic!("More than one shared letter"),
    }
}

fn get_score_for_item(letter: char) -> u32 {
    let value = letter as u32;
    if value >= 'a' as u32 && value <= 'z' as u32 {
        value - 'a' as u32 + 1
    } else if value >= 'A' as u32 && value <= 'Z' as u32 {
        value - 'A' as u32 + 27
    } else {
        panic!("Invalid letter: {}", letter);
    }
}

fn get_score_for_backback(backpack: &Backpack) -> u32 {
    get_score_for_item(get_shared_items(backpack))
}

fn get_shared_item_in_group(group: &Group) -> char {
    let letters1: Vec<char> = group.elf1.chars().collect();
    let letters2: Vec<char> = group.elf2.chars().collect();
    let letters3: Vec<char> = group.elf3.chars().collect();
    let shared = letters1.intersect(letters2).intersect(letters3);
    match shared[..] {
        [letter] => letter,
        [] => panic!("No shared letters"),
        _ => panic!("More than one shared letter"),
    }
}

fn get_score_for_group(group: &Group) -> u32 {
    get_score_for_item(get_shared_item_in_group(group))
}

fn main() {
    let input = read_input();

    // Part 1
    let backpacks = parse_input(input.lines());
    let score: u32 = backpacks
        .into_iter()
        .map(|backpack| get_score_for_backback(&backpack))
        .sum();
    println!("Score - Part 1: {}", score);

    // Part 2
    let groups = parse_input2(input.lines());
    let score: u32 = groups.iter().map(get_score_for_group).sum();
    println!("Score - Part 2: {}", score);
}

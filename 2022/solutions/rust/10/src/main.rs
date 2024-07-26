use std::{env, fs, str::Lines};

fn read_input() -> String {
    let args: Vec<String> = env::args().collect();
    let filename = &args[1];
    fs::read_to_string(filename).expect("Should have been able to read the file")
}

#[derive(Debug)]
enum Command {
    Noop,
    Addx(i32),
}

fn parse_input(lines: Lines<'_>) -> Vec<Command> {
    fn parse_line(line: &str) -> Command {
        let line_parts: Vec<&str> = line.split_whitespace().collect();
        match line_parts[..] {
            ["noop"] => Command::Noop,
            ["addx", value] => Command::Addx(value.parse().unwrap()),
            _ => {
                panic!("Unknown command: {}", line);
            }
        }
    }
    lines.map(parse_line).collect()
}

fn simulate<F>(commands: &[Command], on_cycle_tick: &mut F)
where
    F: FnMut(i32, i32),
{
    let mut cycle = 0;
    let mut register = 1;

    let mut increase_cycle = |register: i32, x| {
        for _ in 0..x {
            cycle += 1;
            on_cycle_tick(register, cycle);
        }
    };

    for command in commands {
        match command {
            Command::Noop => {
                increase_cycle(register, 1);
            }
            Command::Addx(value) => {
                increase_cycle(register, 2);
                register += value;
            }
        }
    }
}

fn draw_image(pixel_data: [[bool; 40]; 6]) {
    for row in pixel_data.iter() {
        for pixel in row.iter() {
            if *pixel {
                print!("#");
            } else {
                print!(".");
            }
        }
        println!();
    }
}

fn main() {
    let input = read_input();
    let commands = parse_input(input.lines());

    // Part 1
    let mut saved_values: Vec<i32> = vec![];
    let mut track_values = |register: i32, cycle: i32| {
        if (cycle - 20) % 40 == 0 {
            saved_values.push(register * cycle);
        }
    };
    simulate(&commands, &mut track_values);
    println!("Part 1: {}", saved_values.iter().sum::<i32>());

    // Part 2
    let mut pixel_data = [[false; 40]; 6];
    let mut calc_image = |register: i32, cycle: i32| {
        let row = (cycle - 1) / 40;
        let column = (cycle - 1) % 40;

        if column == register - 1 || column == register || column == register + 1 {
            pixel_data[row as usize][column as usize] = true;
        }
    };
    simulate(&commands, &mut calc_image);
    println!("Part 2:");
    draw_image(pixel_data);
}

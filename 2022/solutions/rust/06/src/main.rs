use itertools::Itertools;
use std::{env, fs};

fn read_input() -> String {
    let args: Vec<String> = env::args().collect();
    let filename = &args[1];
    fs::read_to_string(filename).expect("Should have been able to read the file")
}

fn are_all_chars_different(input: &str) -> bool {
    let groups = input
        .chars()
        .into_group_map_by(|c| *c)
        .into_iter()
        .collect_vec();
    groups.len() == input.len()
}

fn find_marker(input: &str, size: usize) -> usize {
    let mut current_window = size;
    while current_window <= input.len() {
        let window = &input[current_window - size..current_window];
        if are_all_chars_different(window) {
            break;
        }
        current_window += 1;
    }
    current_window
}

fn main() {
    let input = read_input();
    // We perform the analysis per line
    for line in input.lines() {
        // Part 1
        let start_of_paket = find_marker(line, 4);
        println!("Part 1: {}", start_of_paket);

        // Part 2
        let start_of_paket = find_marker(line, 14);
        println!("Part 2: {}", start_of_paket);
    }
}

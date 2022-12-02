use itertools::Itertools;
use std::fs;

#[derive(Debug)]
pub struct Calories(u32);

#[derive(Debug)]
struct Elf {
    calories: Vec<Calories>,
}

#[derive(Debug)]
struct Input {
    elfs: Vec<Elf>,
}

fn read_input() -> Input {
    let input = fs::read_to_string("input.txt").expect("Should have been able to read the file");
    let lines = input.lines();
    let mut elfs: Vec<Elf> = Vec::new();
    let mut calories: Vec<Calories> = Vec::new();
    for line in lines {
        let trimmed_line = line.trim();
        if trimmed_line.is_empty() {
            elfs.push(Elf { calories });
            calories = Vec::new();
        } else {
            let calories_value = trimmed_line.parse::<u32>().unwrap();
            calories.push(Calories(calories_value));
        }
    }
    // Push last elf
    if !calories.is_empty() {
        elfs.push(Elf { calories });
    }
    Input { elfs }
}

fn find_max_calories(input: &Input) -> Calories {
    let value = input
        .elfs
        .iter()
        // Sum up elfs calories
        .map(|elf| elf.calories.iter().map(|calories| calories.0).sum::<u32>())
        .max()
        .expect("Should have been able to find the max");
    Calories(value)
}

fn find_max_calories_for_x(input: &Input, x: usize) -> Calories {
    let value = input
        .elfs
        .iter()
        // Sum up elfs calories
        .map(|elf| elf.calories.iter().map(|calories| calories.0).sum::<u32>())
        .sorted()
        .rev()
        .take(x)
        .sum();
    Calories(value)
}

fn main() {
    let input = read_input();

    // Part 1
    let max_calories = find_max_calories(&input);
    println!("Max calories on one elf: {:?}", max_calories);

    // Part 2
    let max_calories_three = find_max_calories_for_x(&input, 3);
    println!("Max calories on three elfs: {:?}", max_calories_three);
}

use itertools::Itertools;
use std::{fs, str::Lines};

#[derive(Debug)]
pub struct Calories(u32);

#[derive(Debug)]
struct Elf {
    calories: Vec<Calories>,
}

fn read_input() -> String {
    fs::read_to_string("input.txt").expect("Should have been able to read the file")
}

fn parse_input(input: Lines<'_>) -> Vec<Elf> {
    let mut elves: Vec<Elf> = Vec::new();
    let mut calories: Vec<Calories> = Vec::new();
    for line in input {
        let trimmed_line = line.trim();
        if trimmed_line.is_empty() {
            elves.push(Elf { calories });
            calories = Vec::new();
        } else {
            let calories_value = trimmed_line.parse::<u32>().unwrap();
            calories.push(Calories(calories_value));
        }
    }
    // Push last elf
    if !calories.is_empty() {
        elves.push(Elf { calories });
    }
    elves
}

fn find_max_calories(elves: &[Elf]) -> Calories {
    let value = elves
        .iter()
        // Sum up elfs calories
        .map(|elf| elf.calories.iter().map(|calories| calories.0).sum::<u32>())
        .max()
        .expect("Should have been able to find the max");
    Calories(value)
}

fn find_max_calories_for_x(elves: &[Elf], x: usize) -> Calories {
    let value = elves
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
    // Prepare input
    let input = read_input();
    let elves = parse_input(input.lines());

    // Part 1
    let max_calories = find_max_calories(&elves);
    println!("Calories - Part 1: {:?}", max_calories.0);

    // Part 2
    let max_calories = find_max_calories_for_x(&elves, 3);
    println!("Calories - Part 2: {:?}", max_calories.0);
}

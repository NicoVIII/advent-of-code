use array_tool::vec::Intersect;
use core::str::Lines;
use std::fs;

#[derive(Debug)]
struct Backpack {
    compartment1: String,
    compartment2: String,
}

#[derive(Debug)]
struct Input {
    backpacks: Vec<Backpack>,
}

fn read_input() -> Input {
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

    let input = fs::read_to_string("input.txt").expect("Should have been able to read the file");
    let backpacks = input
        .lines()
        .filter_map(|line| parse_line(line.to_string()))
        .collect();
    Input { backpacks }
}

#[derive(Debug)]
struct Group {
    elf1: String,
    elf2: String,
    elf3: String,
}

#[derive(Debug)]
struct Input2 {
    groups: Vec<Group>,
}

fn read_input2() -> Input2 {
    fn group_up(lines: Lines<'_>) -> Vec<Group> {
        let mut chunk = Vec::new();
        let mut groups = Vec::new();
        for line in lines {
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

    let input = fs::read_to_string("input.txt").expect("Should have been able to read the file");
    let groups = group_up(input.lines());
    Input2 { groups }
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
    let score: u32 = input.backpacks.iter().map(get_score_for_backback).sum();
    println!("Score: {}", score);

    let input2 = read_input2();
    let score2: u32 = input2.groups.iter().map(get_score_for_group).sum();
    println!("Score - Part 2: {}", score2);
}

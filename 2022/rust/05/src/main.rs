use itertools::Itertools;
use std::{
    fs::{self},
    str::Lines,
};

type Stack = Vec<String>;

#[derive(Debug)]
struct Instruction {
    amount: usize,
    from: usize,
    to: usize,
}

#[derive(Debug)]
struct Input {
    stacks: Vec<Stack>,
    instructions: Vec<Instruction>,
}

fn read_input() -> String {
    fs::read_to_string("input.txt").expect("Should have been able to read the file")
}

fn parse_input(lines: Lines<'_>) -> Input {
    fn split_lines(lines: Lines<'_>) -> (Vec<&str>, Vec<&str>) {
        let mut iterator = 0;
        lines
            .group_by(|line| match *line {
                "" => {
                    let old_value = iterator;
                    iterator += 1;
                    old_value
                }
                _ => iterator,
            })
            .into_iter()
            // Remove empty lines and group key
            .map(|(_key, group)| group.filter(|line| !line.is_empty()).collect_vec())
            .collect_tuple()
            .expect("Should have been able to split the lines into two parts")
    }

    fn create_stacks(lines: Vec<&str>) -> Vec<Stack> {
        // We first have a look at the last line to determine how many stacks we have
        let last_line = lines
            .last()
            .expect("Should have been able to get the last line");
        let number_of_stacks = last_line.chars().filter(|c| *c != ' ').count();

        // Initialize the stacks
        let mut stacks: Vec<Stack> = Vec::new();
        for _ in 0..number_of_stacks {
            stacks.push(Vec::new());
        }

        fn find_item_in_line(line: &str, position: usize) -> Option<&str> {
            let index = position * 4 + 1;
            if line.len() <= index {
                None
            } else {
                match &line[index..index + 1] {
                    " " | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" | "0" => None,
                    "[" | "]" => panic!(
                        "Something went wrong, shouldn't find bracket here {} - {}",
                        index, line
                    ),
                    letter => Some(letter),
                }
            }
        }

        fn find_items_in_lines(number_of_stacks: usize, line: &str) -> Vec<Option<&str>> {
            let mut items = Vec::new();
            for position in 0..number_of_stacks {
                items.push(find_item_in_line(line, position));
            }
            items
        }

        for line in lines {
            let items = find_items_in_lines(number_of_stacks, line);
            for position in 0..number_of_stacks {
                if let Some(item) = items[position] {
                    stacks[position].push(item.to_string());
                }
            }
        }

        // Reverse all Stacks to have the order from bottom to top
        for stack in &mut stacks {
            stack.reverse();
        }

        stacks
    }

    fn create_instructions(lines: Vec<&str>) -> Vec<Instruction> {
        lines
            .into_iter()
            .map(|line| {
                let mut iterator = line.split_whitespace();
                assert!(iterator.next().unwrap() == "move");
                let amount = iterator.next().unwrap().parse().unwrap();
                assert!(iterator.next().unwrap() == "from");
                // Adjust the index to start at 0
                let from = iterator.next().unwrap().parse::<usize>().unwrap() - 1;
                assert!(iterator.next().unwrap() == "to");
                // Adjust the index to start at 0
                let to = iterator.next().unwrap().parse::<usize>().unwrap() - 1;
                Instruction { amount, from, to }
            })
            .collect()
    }

    let (stack_lines, instructions_lines) = split_lines(lines);

    let stacks = create_stacks(stack_lines);
    let instructions = create_instructions(instructions_lines);

    Input {
        stacks,
        instructions,
    }
}

fn execute_instruction(stacks: &mut [Stack], instruction: &Instruction, one_by_one: bool) {
    let mut items = stacks[instruction.from]
        .drain(stacks[instruction.from].len() - instruction.amount..)
        .collect_vec();
    assert!(items.len() == instruction.amount);
    if one_by_one {
        items.reverse();
    }
    stacks[instruction.to].append(&mut items);
}

fn execute_instructions(
    stacks: &[Stack],
    instructions: &[Instruction],
    one_by_one: bool,
) -> Vec<Stack> {
    let mut stacks = stacks.to_owned();
    for instruction in instructions {
        execute_instruction(&mut stacks, instruction, one_by_one);
    }
    stacks
}

fn print_result(stacks: &[Stack]) {
    for stack in stacks {
        print!("{}", stack.last().unwrap());
    }
    println!();
}

fn main() {
    let input = read_input();
    let input = parse_input(input.lines());

    // Part 1
    let stacks = execute_instructions(&input.stacks, &input.instructions, true);
    print!("Part 1: ");
    print_result(&stacks);

    // Part 2
    let stacks = execute_instructions(&input.stacks, &input.instructions, false);
    print!("Part 2: ");
    print_result(&stacks);
}

pub mod clicks;
pub mod dial;

use clicks::Clicks;
use dial::Dial;

const START_POSITION: i32 = 50;

#[derive(Clone, Copy, Debug, PartialEq, Eq)]
pub enum Instruction {
    TurnLeft(Clicks),
    TurnRight(Clicks),
}

pub fn parse(input: &str) -> Result<Vec<Instruction>, String> {
    input
        .lines()
        .map(|line| {
            let mut chars = line.chars();
            let direction = chars
                .next()
                .ok_or_else(|| "Invalid instruction format!".to_string())?;
            let clicks = chars.as_str().parse::<Clicks>()?;

            match direction {
                'L' => Ok(Instruction::TurnLeft(clicks)),
                'R' => Ok(Instruction::TurnRight(clicks)),
                _ => Err("Invalid instruction format!".to_string()),
            }
        })
        .collect()
}

pub fn solve_part1(instructions: &[Instruction]) -> String {
    let (_, zeros) = instructions.iter().fold(
        (
            Dial::new(START_POSITION).expect("starting dial position must be valid"),
            0,
        ),
        |(dial, zeros), instruction| {
            let new_dial = match instruction {
                Instruction::TurnLeft(clicks) => dial.turn_left(*clicks),
                Instruction::TurnRight(clicks) => dial.turn_right(*clicks),
            };

            let new_zeros = if new_dial.position() == 0 {
                zeros + 1
            } else {
                zeros
            };

            (new_dial, new_zeros)
        },
    );

    zeros.to_string()
}

pub fn solve_part2(instructions: &[Instruction]) -> String {
    let (_, zeros) = instructions.iter().fold(
        (
            Dial::new(START_POSITION).expect("starting dial position must be valid"),
            0,
        ),
        |(dial, zeros), instruction| {
            let (new_dial, zero_passes) = match instruction {
                Instruction::TurnLeft(clicks) => dial.turn_left_and_count_zero_passes(*clicks),
                Instruction::TurnRight(clicks) => dial.turn_right_and_count_zero_passes(*clicks),
            };

            (new_dial, zeros + zero_passes)
        },
    );

    zeros.to_string()
}

pub fn solve(input: &str) -> Result<(String, String), String> {
    let instructions = parse(input.trim())?;
    let part1 = solve_part1(&instructions);
    let part2 = solve_part2(&instructions);

    Ok((part1, part2))
}
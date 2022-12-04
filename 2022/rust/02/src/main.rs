use core::str::Lines;
use std::fs;

#[derive(Debug)]
enum Hand {
    Rock,
    Paper,
    Scissors,
}

#[derive(Debug)]
struct Round {
    enemy: Hand,
    me: Hand,
}

fn read_input() -> String {
    fs::read_to_string("input.txt").expect("Should have been able to read the file")
}

fn parse_input(input: Lines<'_>) -> Vec<Round> {
    fn parse_line(line: &str) -> Round {
        let mut split = line.split_whitespace();
        let enemy = match split.next().unwrap() {
            "A" => Hand::Rock,
            "B" => Hand::Paper,
            "C" => Hand::Scissors,
            _ => panic!("Unknown hand for enemy"),
        };
        let me = match split.next().unwrap() {
            "X" => Hand::Rock,
            "Y" => Hand::Paper,
            "Z" => Hand::Scissors,
            _ => panic!("Unknown hand for me"),
        };
        Round { enemy, me }
    }

    input.map(parse_line).collect()
}

#[derive(Debug)]
enum Result {
    Win,
    Lose,
    Draw,
}

#[derive(Debug)]
struct Round2 {
    enemy: Hand,
    result: Result,
}

fn parse_input2(input: Lines<'_>) -> Vec<Round2> {
    fn parse_line(line: &str) -> Round2 {
        let mut split = line.split_whitespace();
        let enemy = match split.next().unwrap() {
            "A" => Hand::Rock,
            "B" => Hand::Paper,
            "C" => Hand::Scissors,
            _ => panic!("Unknown hand for enemy"),
        };
        let result = match split.next().unwrap() {
            "X" => Result::Lose,
            "Y" => Result::Draw,
            "Z" => Result::Win,
            _ => panic!("Unknown result"),
        };
        Round2 { enemy, result }
    }

    input.map(parse_line).collect()
}

fn transform_round2_to_round(input: Vec<Round2>) -> Vec<Round> {
    fn result_to_hand(enemy_hand: &Hand, result: &Result) -> Hand {
        match (enemy_hand, result) {
            (Hand::Rock, Result::Win) => Hand::Paper,
            (Hand::Rock, Result::Lose) => Hand::Scissors,
            (Hand::Rock, Result::Draw) => Hand::Rock,
            (Hand::Paper, Result::Win) => Hand::Scissors,
            (Hand::Paper, Result::Lose) => Hand::Rock,
            (Hand::Paper, Result::Draw) => Hand::Paper,
            (Hand::Scissors, Result::Win) => Hand::Rock,
            (Hand::Scissors, Result::Lose) => Hand::Paper,
            (Hand::Scissors, Result::Draw) => Hand::Scissors,
        }
    }

    input
        .into_iter()
        .map(|round| Round {
            me: result_to_hand(&round.enemy, &round.result),
            enemy: round.enemy,
        })
        .collect()
}

fn calculate_outcome_score(round: &Round) -> u32 {
    match (&round.me, &round.enemy) {
        (Hand::Paper, Hand::Rock)
        | (Hand::Scissors, Hand::Paper)
        | (Hand::Rock, Hand::Scissors) => {
            // Win
            6
        }
        (Hand::Paper, Hand::Paper)
        | (Hand::Scissors, Hand::Scissors)
        | (Hand::Rock, Hand::Rock) => {
            // Draw
            3
        }
        (Hand::Paper, Hand::Scissors)
        | (Hand::Scissors, Hand::Rock)
        | (Hand::Rock, Hand::Paper) => {
            // Lose
            0
        }
    }
}

fn calculate_hand_score(round: &Round) -> u32 {
    match &round.me {
        Hand::Rock => 1,
        Hand::Paper => 2,
        Hand::Scissors => 3,
    }
}

fn calculate_score(round: &Round) -> u32 {
    calculate_outcome_score(round) + calculate_hand_score(round)
}

fn main() {
    let input = read_input();

    // Part 1
    let rounds = parse_input(input.lines());
    let score: u32 = rounds
        .into_iter()
        .map(|round| calculate_score(&round))
        .sum();
    println!("Score - Part 1: {}", score);

    // Part 2
    let rounds = parse_input2(input.lines());
    let rounds = transform_round2_to_round(rounds);
    let score2: u32 = rounds.iter().map(calculate_score).sum();
    println!("Score - Part 2: {}", score2);
}

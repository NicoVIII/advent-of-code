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

#[derive(Debug)]
struct Input {
    rounds: Vec<Round>,
}

fn read_input() -> Input {
    fn parse_line(line: String) -> Round {
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

    let input = fs::read_to_string("input.txt").expect("Should have been able to read the file");
    let lines = input.lines();
    let rounds = lines.map(|line| parse_line(line.to_string())).collect();
    Input { rounds }
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

#[derive(Debug)]
struct Input2 {
    rounds: Vec<Round2>,
}

fn read_input_2() -> Input2 {
    fn parse_line(line: String) -> Round2 {
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

    let input = fs::read_to_string("input.txt").expect("Should have been able to read the file");
    let lines = input.lines();
    let rounds = lines.map(|line| parse_line(line.to_string())).collect();
    Input2 { rounds }
}

fn transform_input_2(input: Input2) -> Input {
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

    let rounds = input
        .rounds
        .into_iter()
        .map(|round| Round {
            me: result_to_hand(&round.enemy, &round.result),
            enemy: round.enemy,
        })
        .collect();
    Input { rounds }
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
    let score: u32 = input.rounds.iter().map(calculate_score).sum();
    println!("Score: {}", score);

    let input2 = read_input_2();
    let input = transform_input_2(input2);
    let score2: u32 = input.rounds.iter().map(calculate_score).sum();
    println!("Real score: {}", score2);
}

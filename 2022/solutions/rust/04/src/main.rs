use array_tool::vec::Intersect;
use std::{env, fs, str::Lines};

fn read_input() -> String {
    let args: Vec<String> = env::args().collect();
    let filename = &args[1];
    fs::read_to_string(filename).expect("Should have been able to read the file")
}

#[derive(Debug)]
struct SectionAssignment {
    start: u8,
    end: u8,
}

type AssignmentPair = (SectionAssignment, SectionAssignment);

fn parse_input(input: Lines<'_>) -> Vec<AssignmentPair> {
    fn parse_assignment(input: &str) -> SectionAssignment {
        let borders: Vec<u8> = input.split('-').map(|s| s.parse::<u8>().unwrap()).collect();
        match borders[..] {
            [start, end] => SectionAssignment { start, end },
            _ => panic!("Invalid input"),
        }
    }

    fn parse_line(line: &str) -> Option<AssignmentPair> {
        let trimmed = line.trim();
        if !trimmed.is_empty() {
            let assignments: Vec<SectionAssignment> = trimmed
                .split(',')
                .into_iter()
                .map(parse_assignment)
                .collect();

            assert!(assignments.len() == 2);
            let mut iter = assignments.into_iter();
            Some((iter.next().unwrap(), iter.next().unwrap()))
        } else {
            None
        }
    }

    input.filter_map(parse_line).collect()
}

fn is_fully_containing_pair(pair: &AssignmentPair) -> bool {
    fn contains(first: &SectionAssignment, second: &SectionAssignment) -> bool {
        first.start <= second.start && first.end >= second.end
    }
    contains(&pair.0, &pair.1) || contains(&pair.1, &pair.0)
}

fn is_overlapping_pair(pair: &AssignmentPair) -> bool {
    let range1 = pair.0.start..=pair.0.end;
    let range2 = pair.1.start..=pair.1.end;
    let intersection = range1.collect::<Vec<u8>>().intersect(range2.collect());
    !intersection.is_empty()
}

fn main() {
    let input = read_input();
    let assignments = parse_input(input.lines());

    // Part 1
    let pairs: u16 = assignments
        .iter()
        .map(|p| is_fully_containing_pair(p) as u16)
        .sum();
    println!("Pairs - Part 1: {}", pairs);

    // Part 2
    let pairs: u16 = assignments
        .iter()
        .map(|p| is_overlapping_pair(p) as u16)
        .sum();
    println!("Pairs - Part 2: {}", pairs);
}

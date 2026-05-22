use std::path::Path;

use aoc::run_for_testset;
use day01::{parse, solve, solve_part1, solve_part2, Instruction};

#[test]
fn parse_rejects_invalid_clicks() {
    assert_eq!(
        parse("L0").unwrap_err(),
        "Invalid clicks value given: 0!".to_string()
    );
}

#[test]
fn part1_counts_only_landings_on_zero() {
    let instructions = parse("R50\nR100\nL50").unwrap();

    assert_eq!(solve_part1(&instructions), "2");
}

#[test]
fn part2_counts_boundary_crossings_and_full_rounds() {
    let instructions = parse("R60\nR100\nL250").unwrap();

    assert_eq!(solve_part2(&instructions), "5");
}

#[test]
fn solve_returns_both_parts() {
    assert_eq!(solve("R50").unwrap(), ("1".to_string(), "1".to_string()));
}

#[test]
fn testset_matches_example_and_puzzle_outputs() {
    run_for_testset(
        Path::new(env!("CARGO_MANIFEST_DIR")),
        "01",
        parse,
        Some(|instructions: &Vec<Instruction>| solve_part1(instructions.as_slice())),
        Some(|instructions: &Vec<Instruction>| solve_part2(instructions.as_slice())),
    )
    .unwrap();
}
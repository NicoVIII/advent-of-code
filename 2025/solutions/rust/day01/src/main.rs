fn main() {
    aoc::run_cli(
        day01::parse,
        |instructions| day01::solve_part1(instructions.as_slice()),
        |instructions| day01::solve_part2(instructions.as_slice()),
    )
    .unwrap_or_else(|error| panic!("{error}"));
}

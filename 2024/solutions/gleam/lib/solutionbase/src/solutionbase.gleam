import argv
import gleam/io
import simplifile

pub type PuzzleSolution(a) {
  PuzzleSolution
}

pub fn run(
  preprocess: fn(String) -> a,
  solve_part1: fn(a) -> String,
  solve_part2: fn(a) -> String,
) {
  case argv.load().arguments {
    [input_file] -> {
      let assert Ok(input) = simplifile.read(input_file)
      let preprocessed_data = preprocess(input)
      io.println("Part 1: " <> solve_part1(preprocessed_data))
      io.println("Part 2: " <> solve_part2(preprocessed_data))
    }
    _ -> io.println("Usage: gleam run -- <input-file>")
  }
}

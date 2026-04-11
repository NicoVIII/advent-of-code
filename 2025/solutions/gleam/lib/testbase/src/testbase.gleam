import gleam/io
import gleam/list
import gleam/option.{type Option, None, Some}
import gleam/string
import gleeunit/should
import simplifile

fn get_output_filename(input_filename) {
  string.replace(input_filename, each: "-input", with: "-output")
}

pub fn run_for_testset(
  preprocess preprocess: fn(String) -> a,
  solve_part1 solve_part1: Option(fn(a) -> String),
  solve_part2 solve_part2: Option(fn(a) -> String),
  day day: String,
) {
  let basepath = "../../../puzzles/" <> day <> "/"
  io.println("Testing with inputs from: " <> basepath)
  let assert Ok(entries) = simplifile.read_directory(basepath)
  list.filter(entries, fn(entry) { string.ends_with(entry, "-input.txt") })
  |> list.each(fn(input_file) {
    io.println("Testing: " <> input_file)
    let assert Ok(input) = simplifile.read(basepath <> input_file)
    let output_file = get_output_filename(input_file)
    let assert Ok(output) = simplifile.read(basepath <> output_file)
    let assert [part1_expected, part2_expected] =
      output
      |> string.trim
      |> string.split("\n")
      |> list.map(fn(line) {
        let trimmed = string.trim(line)
        case trimmed {
          "-" -> None
          x -> Some(x)
        }
      })

    // Check if we have anything to test
    case solve_part1, solve_part2, part1_expected, part2_expected {
      None, None, _, _ -> {
        io.println("Error: No parts to test")
        io.println_error("No parts to test")
        panic
      }
      _, _, None, None -> {
        io.println("Error: No expected results to test against")
        io.println_error("No expected results to test against")
        panic
      }
      _, _, _, _ -> Nil
    }

    let preprocessed_data = input |> string.trim() |> preprocess()
    case solve_part1, part1_expected {
      Some(part1), Some(expected) -> {
        let actual = part1(preprocessed_data)
        io.println("Part 1: " <> actual)
        should.equal(actual, expected)
      }
      _, _ -> Nil
    }
    case solve_part2, part2_expected {
      Some(part2), Some(expected) -> {
        let actual = part2(preprocessed_data)
        io.println("Part 2: " <> actual)
        should.equal(actual, expected)
      }
      _, _ -> Nil
    }
  })
}

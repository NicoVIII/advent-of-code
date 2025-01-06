import gleam/io
import gleam/list
import gleam/option.{type Option, None, Some}
import gleam/string
import gleeunit/should
import simplifile

pub type Config {
  Config(
    day: String,
    part1: Option(fn(String) -> String),
    part2: Option(fn(String) -> String),
  )
}

fn get_output_filename(input_filename) {
  string.replace(input_filename, each: "-input", with: "-output")
}

pub fn testset_test(config: Config) {
  let basepath = "../../../puzzles/" <> config.day <> "/"
  let assert Ok(entries) = simplifile.read_directory(basepath)
  list.filter(entries, fn(entry) { string.ends_with(entry, "-input.txt") })
  |> list.each(fn(input_file) {
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
    case config.part1, part1_expected {
      Some(part1), Some(expected) -> {
        let actual = part1(input)
        io.println("Part 1: " <> actual)
        should.equal(actual, expected)
      }
      _, _ -> Nil
    }
    case config.part2, part2_expected {
      Some(part2), Some(expected) -> {
        let actual = part2(input)
        io.println("Part 2: " <> actual)
        should.equal(actual, expected)
      }
      _, _ -> Nil
    }
  })
}

import argv
import gleam/int
import gleam/io
import gleam/list
import gleam/string
import simplifile

type Level =
  Int

type Report =
  List(Level)

type Data =
  List(Report)

pub fn parse(input: String) -> Data {
  input
  |> string.trim
  |> string.split("\n")
  |> list.map(fn(line) {
    string.trim(line)
    |> string.split(" ")
    |> list.map(fn(entry) {
      let assert Ok(level) = int.parse(entry)
      level
    })
  })
}

fn is_report_valid(report: Report) {
  let all_increasing = list.sort(report, int.compare) == report
  let all_decreasing =
    list.sort(report, fn(a, b) { int.compare(b, a) }) == report
  let no_jumps =
    list.window_by_2(report)
    |> list.all(fn(window) {
      let #(a, b) = window
      let diff = int.absolute_value(a - b)
      diff >= 1 && diff <= 3
    })
  { all_increasing || all_decreasing } && no_jumps
}

pub fn part1(reports: Data) -> Int {
  // Calculate
  list.count(reports, is_report_valid)
}

pub fn part2(reports: Data) -> Int {
  list.count(reports, fn(report) {
    // Generate tolerated reports
    list.range(0, list.length(report) - 1)
    |> list.map(fn(i) {
      list.append(list.take(report, i), list.drop(report, i + 1))
    })
    |> list.any(is_report_valid)
  })
}

pub fn main() {
  case argv.load().arguments {
    [input_file] -> {
      let assert Ok(input) = simplifile.read(input_file)
      let data = parse(input)
      io.println("Part 1: " <> { part1(data) |> int.to_string })
      io.println("Part 2: " <> { part2(data) |> int.to_string })
    }
    _ -> io.println("Usage: gleam run -- <input-file>")
  }
}

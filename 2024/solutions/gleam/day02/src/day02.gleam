import gleam/int
import gleam/list
import gleam/string
import solutionbase

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

pub fn solve_part1(reports: Data) -> String {
  // Calculate
  list.count(reports, is_report_valid)
  |> int.to_string
}

pub fn solve_part2(reports: Data) -> String {
  list.count(reports, fn(report) {
    // Generate tolerated reports
    int.range(list.length(report), -1, [], list.prepend)
    |> list.map(fn(i) {
      list.append(list.take(report, i), list.drop(report, i + 1))
    })
    |> list.any(is_report_valid)
  })
  |> int.to_string
}

pub fn main() {
  solutionbase.run(parse, solve_part1, solve_part2)
}

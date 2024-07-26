import gleam/int
import gleam/io
import gleam/list
import gleam/result
import gleam/string
import simplifile

fn parse_calories(input: String) -> List(Int) {
  input
  |> string.split(on: "\n")
  |> list.map(fn(calorie_string) {
    let assert Ok(value) = int.parse(calorie_string)
    value
  })
}

pub fn main() {
  let assert Ok(input) = simplifile.read("input.txt")
  input
  |> string.split(on: "\n\n")
  |> list.map(fn(backpack_content) {
    parse_calories(backpack_content)
    |> int.sum
  })
  |> list.reduce(int.max)
  |> case {
    Ok(value) -> value
    _ -> panic
  }
  |> int.to_string
  |> io.println
}

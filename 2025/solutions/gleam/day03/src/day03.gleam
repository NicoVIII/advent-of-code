import battery_joltage
import gleam/int
import gleam/list
import gleam/pair
import gleam/string
import solutionbase

type Bank =
  List(battery_joltage.T)

pub fn parse(input: String) -> List(Bank) {
  input
  |> string.split("\n")
  |> list.map(fn(line) {
    string.to_graphemes(line)
    |> list.map(fn(grapheme) {
      let assert Ok(joltage_value) = int.parse(grapheme)
      battery_joltage.new_exn(joltage_value)
    })
  })
}

fn find_highest_joltage(
  rest: List(Int),
  max_with_tail: #(Int, List(Int)),
) -> #(Int, List(Int)) {
  let #(max, _) = max_with_tail
  case rest {
    // Rest is empty or only one element left, we return the current max
    [] | [_] -> max_with_tail
    // We found a bigger joltage value, we update the max and the tail and search further
    [current, ..tail] if current > max ->
      #(current, tail) |> find_highest_joltage(tail, _)
    // Nothing new, keep searching
    [_, ..tail] -> find_highest_joltage(tail, max_with_tail)
  }
}

fn max_joltage_for_bank(bank: Bank) -> Int {
  bank
  |> list.map(battery_joltage.value)
  // We find the highest joltage value and the tail behind it in the bank
  |> find_highest_joltage(#(0, []))
  // Next we find the highest joltage value in the tail and compose both numbers
  |> pair.map_second(list.max(_, int.compare))
  |> fn(numbers) {
    let assert #(first, Ok(second)) = numbers
    first * 10 + second
  }
}

pub fn solve_part1(data: List(Bank)) -> String {
  data
  |> list.map(max_joltage_for_bank)
  |> list.fold(from: 0, with: int.add)
  |> int.to_string
}

pub fn main() -> Nil {
  solutionbase.run(parse, solve_part1, fn(_data) { "-" })
}

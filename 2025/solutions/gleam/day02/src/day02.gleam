import gleam/bool
import gleam/int
import gleam/list
import gleam/string
import id
import id_range
import solutionbase

type Data =
  List(id_range.T)

pub fn parse(input: String) -> Data {
  let parse_line = fn(line: String) -> id_range.T {
    let assert [from, to] = string.split(line, "-")
    let assert Ok(from) = int.parse(from)
    let assert Ok(to) = int.parse(to)
    id_range.new_exn(id.new_exn(from), to: id.new_exn(to))
  }

  input
  |> string.split(",")
  |> list.map(parse_line)
}

pub fn is_invalid_id_for_part1(id: id.T) -> Bool {
  let id_string = id |> id.value |> int.to_string
  let id_string_length = string.length(id_string)
  assert id_string_length >= 1
  use <- bool.guard(id_string_length % 2 != 0, False)

  let first_half =
    string.slice(id_string, at_index: 0, length: id_string_length / 2)
  let second_half =
    string.slice(
      id_string,
      at_index: id_string_length / 2,
      length: id_string_length / 2,
    )
  first_half == second_half
}

pub fn is_invalid_id_for_part2(id: id.T) -> Bool {
  let id_string = id |> id.value |> int.to_string
  let id_string_length = string.length(id_string)
  assert id_string_length >= 1
  // We go through all possible divisors of the id lengths - +1 because to is exclusive
  int.range(
    from: 2,
    to: id_string_length + 1,
    with: False,
    run: fn(is_invalid, divisor) {
      // If we already know that the id is invalid, we can skip the rest of the checks
      use <- bool.guard(is_invalid, True)
      // If the potential divisor is not a divisor of the id length, we can skip the check
      use <- bool.guard(id_string_length % divisor != 0, False)

      let slice = string.slice(id_string, 0, id_string_length / divisor)
      let expected_string = string.repeat(slice, divisor)
      id_string == expected_string
    },
  )
}

pub fn solve(data: Data, is_id_invalid: fn(id.T) -> Bool) {
  data
  |> list.flat_map(fn(range) {
    id_range.fold(range, from: [], with: fn(invalid_ids, id) {
      case is_id_invalid(id) {
        True -> [id, ..invalid_ids]
        False -> invalid_ids
      }
    })
  })
  |> list.map(id.value)
  |> echo
  |> list.fold(from: 0, with: int.add)
  |> int.to_string()
}

pub fn solve_part1(data: Data) {
  solve(data, is_invalid_id_for_part1)
}

pub fn solve_part2(data: Data) {
  solve(data, is_invalid_id_for_part2)
}

pub fn main() -> Nil {
  solutionbase.run(parse, solve_part1, solve_part2)
}

import clicks
import dial
import gleam/int
import gleam/list
import gleam/pair
import gleam/string
import solutionbase

pub type Instruction {
  TurnLeft(clicks.T)
  TurnRight(clicks.T)
}

pub fn parse(input: String) -> List(Instruction) {
  input
  |> string.trim()
  |> string.split("\n")
  |> list.map(fn(line) {
    let assert Ok(parts) = string.pop_grapheme(line)
    case parts {
      #("L", clicks) -> {
        let assert Ok(clicks) = int.parse(clicks)
        TurnLeft(clicks.new_exn(clicks))
      }
      #("R", clicks) -> {
        let assert Ok(clicks) = int.parse(clicks)
        TurnRight(clicks.new_exn(clicks))
      }
      _ -> panic as "Invalid instruction format!"
    }
  })
}

pub fn solve_part1(instructions: List(Instruction)) -> String {
  list.fold(instructions, #(dial.new_exn(50), 0), fn(acc, instruction) -> #(
    dial.T,
    Int,
  ) {
    let #(dial, zeros) = acc
    let new_dial = case instruction {
      TurnLeft(clicks) -> dial.turn_left(dial, clicks)
      TurnRight(clicks) -> dial.turn_right(dial, clicks)
    }
    case dial.position(new_dial) {
      0 -> #(new_dial, zeros + 1)
      _ -> #(new_dial, zeros)
    }
  })
  |> pair.second
  |> int.to_string()
}

pub fn solve_part2(instructions: List(Instruction)) -> String {
  list.fold(instructions, #(dial.new_exn(50), 0), fn(acc, instruction) -> #(
    dial.T,
    Int,
  ) {
    let #(dial, zeros) = acc
    let #(new_dial, zero_passes) = case instruction {
      TurnLeft(clicks) -> dial.turn_left_and_count_zero_passes(dial, clicks)
      TurnRight(clicks) -> dial.turn_right_and_count_zero_passes(dial, clicks)
    }
    #(new_dial, zeros + zero_passes)
  })
  |> pair.second
  |> int.to_string()
}

pub fn main() -> Nil {
  solutionbase.run(parse, solve_part1, fn(_data) { "-" })
}

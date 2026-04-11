import clicks
import gleam/int

pub opaque type T {
  Dial(position: Int)
}

// Nicer type alias for unqualified usage/imports
pub type Dial =
  T

pub fn position(dial: Dial) -> Int {
  dial.position
}

pub fn new(position: Int) -> Result(Dial, Nil) {
  case position {
    pos if pos < 0 || pos > 99 -> Error(Nil)
    _ -> Ok(Dial(position))
  }
}

pub fn new_exn(position: Int) -> Dial {
  case new(position) {
    Ok(dial_position) -> dial_position
    Error(Nil) ->
      panic as {
        "Invalid dial position given: " <> int.to_string(position) <> "!"
      }
  }
}

fn turn(dial: Dial, change: Int) -> Dial {
  let assert Ok(new_position) = int.modulo(position(dial) + change, 100)
  new_exn(new_position)
}

pub fn turn_left(dial: Dial, clicks: clicks.T) -> Dial {
  turn(dial, -clicks.value(clicks))
}

pub fn turn_right(dial: Dial, clicks: clicks.T) -> Dial {
  turn(dial, clicks.value(clicks))
}

fn turn_and_count_zero_passes(dial: Dial, change: Int) -> #(Dial, Int) {
  let absolute_change = int.absolute_value(change)
  let assert Ok(full_rounds) = absolute_change |> int.divide(by: 100)
  let normalized_change = change % 100
  let zero_passes = case dial.position {
    0 -> full_rounds
    _ ->
      case dial.position + normalized_change {
        // If we pass again, we also count that
        x if x <= 0 || x > 99 -> full_rounds + 1
        _ -> full_rounds
      }
  }

  let new_dial = turn(dial, normalized_change)
  #(new_dial, zero_passes)
}

pub fn turn_left_and_count_zero_passes(
  dial: Dial,
  clicks: clicks.T,
) -> #(Dial, Int) {
  turn_and_count_zero_passes(dial, -clicks.value(clicks))
}

pub fn turn_right_and_count_zero_passes(
  dial: Dial,
  clicks: clicks.T,
) -> #(Dial, Int) {
  turn_and_count_zero_passes(dial, clicks.value(clicks))
}

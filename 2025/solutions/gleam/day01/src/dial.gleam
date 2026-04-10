import gleam/int
import gleam/option.{type Option, None, Some}

pub opaque type T {
  Dial(position: Int)
}

pub fn value(dial: T) -> Int {
  dial.position
}

pub fn new(position: Int) -> Option(T) {
  case position {
    pos if pos < 0 || pos > 99 -> None
    _ -> Some(Dial(position))
  }
}

pub fn new_exn(position: Int) -> T {
  case new(position) {
    None ->
      panic as {
        "Invalid dial position given: " <> int.to_string(position) <> "!"
      }
    Some(dial_position) -> dial_position
  }
}

fn change(dial: T, change: Int) -> T {
  let assert Ok(new_value) = int.modulo(value(dial) + change, 100)
  new_exn(new_value)
}

pub fn turn_left(dial: T, clicks: Int) -> T {
  change(dial, -clicks)
}

pub fn turn_right(dial: T, clicks: Int) -> T {
  change(dial, clicks)
}

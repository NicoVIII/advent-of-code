import gleam/int
import gleam/option.{type Option, None, Some}

pub opaque type T {
  T(value: Int)
}

pub fn new(value: Int) -> Option(T) {
  case value <= 0 {
    True -> None
    False -> Some(T(value))
  }
}

pub fn new_exn(value: Int) -> T {
  case new(value) {
    None ->
      panic as { "Invalid clicks value given: " <> int.to_string(value) <> "!" }
    Some(clicks) -> clicks
  }
}

pub fn value(clicks: T) -> Int {
  clicks.value
}

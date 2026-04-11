import gleam/int

pub opaque type T {
  Clicks(value: Int)
}

// Nicer type alias for unqualified usage/imports
pub type Clicks =
  T

pub fn new(value: Int) -> Result(Clicks, Nil) {
  case value <= 0 {
    True -> Error(Nil)
    False -> Ok(Clicks(value))
  }
}

pub fn new_exn(value: Int) -> Clicks {
  case new(value) {
    Ok(clicks) -> clicks
    Error(Nil) ->
      panic as { "Invalid clicks value given: " <> int.to_string(value) <> "!" }
  }
}

pub fn value(clicks: Clicks) -> Int {
  clicks.value
}

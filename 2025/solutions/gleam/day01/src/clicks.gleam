import gleam/int

pub opaque type T {
  T(value: Int)
}

pub fn new(value: Int) -> Result(T, Nil) {
  case value <= 0 {
    True -> Error(Nil)
    False -> Ok(T(value))
  }
}

pub fn new_exn(value: Int) -> T {
  case new(value) {
    Ok(clicks) -> clicks
    Error(Nil) ->
      panic as { "Invalid clicks value given: " <> int.to_string(value) <> "!" }
  }
}

pub fn value(clicks: T) -> Int {
  clicks.value
}

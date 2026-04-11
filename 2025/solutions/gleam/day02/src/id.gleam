import gleam/int

pub opaque type T {
  T(value: Int)
}

pub fn new(value: Int) -> Result(T, Nil) {
  case value >= 0 {
    True -> Ok(T(value: value))
    False -> Error(Nil)
  }
}

pub fn new_exn(value: Int) -> T {
  case new(value) {
    Ok(id) -> id
    Error(Nil) -> panic as { "Invalid Id value: " <> int.to_string(value) }
  }
}

pub fn value(id: T) -> Int {
  id.value
}

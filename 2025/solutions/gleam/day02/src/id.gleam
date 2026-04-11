import gleam/int

pub opaque type T {
  Id(value: Int)
}

// Nicer type alias for unqualified usage/imports
pub type Id =
  T

pub fn new(value: Int) -> Result(Id, Nil) {
  case value >= 0 {
    True -> Ok(Id(value: value))
    False -> Error(Nil)
  }
}

pub fn new_exn(value: Int) -> Id {
  case new(value) {
    Ok(id) -> id
    Error(Nil) -> panic as { "Invalid Id value: " <> int.to_string(value) }
  }
}

pub fn value(id: Id) -> Int {
  id.value
}

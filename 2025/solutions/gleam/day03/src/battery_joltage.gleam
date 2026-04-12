import gleam/int

pub opaque type T {
  BatteryJoltage(value: Int)
}

// Nicer type alias for unqualified usage/imports
pub type BatteryJoltage =
  T

pub fn new(value: Int) -> Result(BatteryJoltage, Nil) {
  case value > 0 && value <= 9 {
    True -> Ok(BatteryJoltage(value))
    False -> Error(Nil)
  }
}

pub fn new_exn(value: Int) -> BatteryJoltage {
  case new(value) {
    Ok(battery_joltage) -> battery_joltage
    Error(_) ->
      panic as { "Invalid battery joltage value: " <> int.to_string(value) }
  }
}

pub fn value(joltage: BatteryJoltage) -> Int {
  joltage.value
}

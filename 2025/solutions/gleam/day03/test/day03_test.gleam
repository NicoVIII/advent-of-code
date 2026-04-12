import gleam/option.{Some}
import gleeunit

import day03
import testbase

pub fn testset_test() {
  testbase.run_for_testset(
    day03.parse,
    Some(day03.solve_part1),
    option.None,
    day: "03",
  )
}

pub fn main() {
  gleeunit.main()
}

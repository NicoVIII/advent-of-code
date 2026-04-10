import gleam/option.{Some}
import gleeunit

import day01
import testbase

pub fn testset_test() {
  testbase.run_for_testset(
    day01.parse,
    Some(day01.solve_part1),
    Some(day01.solve_part2),
    day: "01",
  )
}

pub fn main() {
  gleeunit.main()
}

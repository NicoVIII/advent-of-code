import gleam/option.{Some}
import gleeunit

import day04
import testbase

pub fn testset_test() {
  testbase.run_for_testset(
    day04.parse,
    Some(day04.solve_part1),
    Some(day04.solve_part2),
    day: "04",
  )
}

pub fn main() {
  gleeunit.main()
}

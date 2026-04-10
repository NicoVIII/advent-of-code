import gleam/option.{Some}
import gleeunit
import testbase

import day02

pub fn testset_test() {
  testbase.run_for_testset(
    day02.parse,
    Some(day02.solve_part1),
    Some(day02.solve_part2),
    day: "02",
  )
}

pub fn main() {
  gleeunit.main()
}

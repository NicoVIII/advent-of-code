import gleam/function.{identity}
import gleam/option.{Some}
import gleeunit

import day03
import testbase

pub fn testset_test() {
  testbase.run_for_testset(
    identity,
    Some(day03.solve_part1),
    Some(day03.solve_part2),
    day: "03",
  )
}

pub fn main() {
  gleeunit.main()
}

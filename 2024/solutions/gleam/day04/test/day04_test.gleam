import gleam/int
import gleam/option.{None, Some}
import gleeunit

import day04
import testbase

pub fn testset_test() {
  let config =
    testbase.Config(
      day: "04",
      part1: Some(fn(input) {
        day04.parse(input) |> day04.part1 |> int.to_string
      }),
      part2: None,
    )
  testbase.testset_test(config)
}

pub fn main() {
  gleeunit.main()
}

import gleam/int
import gleam/option.{Some}
import gleeunit

import day03
import testbase

pub fn testset_test() {
  let config =
    testbase.Config(
      day: "03",
      part1: Some(fn(input) {
        day03.parse(input) |> day03.part1 |> int.to_string
      }),
      part2: option.None,
    )
  testbase.testset_test(config)
}

pub fn main() {
  gleeunit.main()
}

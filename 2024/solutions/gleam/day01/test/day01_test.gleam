import gleam/int
import gleam/option.{Some}
import gleeunit

import day01
import testbase

pub fn testset_test() {
  let config =
    testbase.Config(
      day: "01",
      part1: Some(fn(input) {
        day01.parse(input) |> day01.part1 |> int.to_string
      }),
      part2: option.None,
    )
  testbase.testset_test(config)
}

pub fn main() {
  gleeunit.main()
}

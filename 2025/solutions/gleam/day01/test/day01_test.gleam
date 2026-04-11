import gleam/option.{Some}
import gleeunit

import day01
import dial
import testbase

pub fn turn_left_and_count_zero_passes_test() {
  let dial = dial.new_exn(50)
  let #(new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(dial, 25)
  assert dial.position(new_dial) == 25
  assert zero_passes == 0

  let #(new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(dial, 125)
  assert dial.position(new_dial) == 25
  assert zero_passes == 1

  let #(new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(dial, 75)
  assert dial.position(new_dial) == 75
  assert zero_passes == 1

  let #(new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(dial, 175)
  assert dial.position(new_dial) == 75
  assert zero_passes == 2

  let #(new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(dial, 50)
  assert dial.position(new_dial) == 0
  assert zero_passes == 1

  let #(new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(dial, 150)
  assert dial.position(new_dial) == 0
  assert zero_passes == 2

  let zero_dial = dial.new_exn(0)
  let #(new_dial, zero_passes) =
    dial.turn_left_and_count_zero_passes(zero_dial, 50)
  assert dial.position(new_dial) == 50
  assert zero_passes == 0
}

pub fn turn_right_and_count_zero_passes_test() {
  let dial = dial.new_exn(50)
  let #(new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(dial, 25)
  assert dial.position(new_dial) == 75
  assert zero_passes == 0

  let #(new_dial, zero_passes) =
    dial.turn_right_and_count_zero_passes(dial, 125)
  assert dial.position(new_dial) == 75
  assert zero_passes == 1

  let #(new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(dial, 75)
  assert dial.position(new_dial) == 25
  assert zero_passes == 1

  let #(new_dial, zero_passes) =
    dial.turn_right_and_count_zero_passes(dial, 175)
  assert dial.position(new_dial) == 25
  assert zero_passes == 2

  let #(new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(dial, 50)
  assert dial.position(new_dial) == 0
  assert zero_passes == 1

  let #(new_dial, zero_passes) =
    dial.turn_right_and_count_zero_passes(dial, 150)
  assert dial.position(new_dial) == 0
  assert zero_passes == 2

  let zero_dial = dial.new_exn(0)
  let #(new_dial, zero_passes) =
    dial.turn_right_and_count_zero_passes(zero_dial, 50)
  assert dial.position(new_dial) == 50
  assert zero_passes == 0
}

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

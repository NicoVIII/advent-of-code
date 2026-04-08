import argv
import gleam/int
import gleam/io
import gleam/list
import gleam/string
import simplifile
import splitter.{type Splitter}

type Data =
  List(#(Int, Int))

type ParsingData {
  ParsingData(enabled: Bool, mul_pairs: Data)
}

type Patterns {
  Patterns(start: Splitter, end: Splitter)
}

fn build_patterns1() -> Patterns {
  Patterns(start: splitter.new(["mul("]), end: splitter.new([")"]))
}

fn parse_pass1(to_parse, data: Data, patterns: Patterns) {
  // Search for the start pattern
  let start_result = splitter.split_after(patterns.start, to_parse)
  case start_result {
    // No more matches, return the data
    #(_, "") -> data
    #(_, candidate) -> {
      // We got a potential candidate, we have to search for a matching end pattern
      let end_result = splitter.split_before(patterns.end, candidate)
      case end_result {
        #(_, "") -> data
        // No more matches, return the data
        #(param_str, _) -> {
          // Used to start the next pass
          let check_next = parse_pass1(candidate, _, patterns)

          // Try to parse params
          let params = string.split_once(param_str, ",")
          case params {
            Error(_) ->
              // No match, continue searching
              check_next(data)
            Ok(split_result) -> {
              let #(first, second) = split_result
              let x_result = int.parse(first)
              let y_result = int.parse(second)
              case x_result, y_result {
                Ok(x), Ok(y) ->
                  // We got a valid pair, add it to the data and continue searching
                  check_next([#(x, y), ..data])
                _, _ ->
                  // No match, continue searching
                  check_next(data)
              }
            }
          }
        }
      }
    }
  }
}

pub fn parse1(input: String) -> Data {
  let patterns = build_patterns1()

  string.trim(input)
  |> parse_pass1([], patterns)
}

fn build_patterns2() -> Patterns {
  Patterns(
    start: splitter.new(["mul(", "do()", "don't()"]),
    end: splitter.new([")"]),
  )
}

fn parse_pass2(to_parse, data: ParsingData, patterns: Patterns) {
  // Search for the start pattern
  let start_result = splitter.split(patterns.start, to_parse)
  case start_result {
    // No more matches, return the data
    #(_, _, "") -> data
    #(_, "do()", next) ->
      parse_pass2(next, ParsingData(..data, enabled: True), patterns)
    #(_, "don't()", next) ->
      parse_pass2(next, ParsingData(..data, enabled: False), patterns)
    #(_, "mul(", candidate) -> {
      // Check if we're enabled, if not just continue searching
      case data.enabled {
        False -> parse_pass2(candidate, data, patterns)
        True -> {
          // We got a potential candidate, we have to search for a matching end pattern
          let end_result = splitter.split_before(patterns.end, candidate)
          case end_result {
            #(_, "") -> data
            // No more matches, return the data
            #(param_str, _) -> {
              // Used to start the next pass
              let check_next = parse_pass2(candidate, _, patterns)

              // Try to parse params
              let params = string.split_once(param_str, ",")
              case params {
                Error(_) ->
                  // No match, continue searching
                  check_next(data)
                Ok(split_result) -> {
                  let #(first, second) = split_result
                  let x_result = int.parse(first)
                  let y_result = int.parse(second)
                  case x_result, y_result {
                    Ok(x), Ok(y) ->
                      // We got a valid pair, add it to the data and continue searching
                      check_next(
                        ParsingData(..data, mul_pairs: [
                          #(x, y),
                          ..data.mul_pairs
                        ]),
                      )
                    _, _ ->
                      // No match, continue searching
                      check_next(data)
                  }
                }
              }
            }
          }
        }
      }
    }
    #(_, _, _) -> panic
  }
}

pub fn parse2(input: String) -> Data {
  let patterns = build_patterns2()

  string.trim(input)
  |> parse_pass2(ParsingData(True, []), patterns)
  |> fn(parsing_data) { parsing_data.mul_pairs }
}

pub fn calculate(data: Data) {
  data
  |> list.map(fn(pair) {
    let #(x, y) = pair
    x * y
  })
  |> list.fold(0, int.add)
}

pub fn main() {
  case argv.load().arguments {
    [input_file] -> {
      let assert Ok(input) = simplifile.read(input_file)
      let data1 = parse1(input)
      io.println("Part 1: " <> { calculate(data1) |> int.to_string })
      let data2 = parse2(input)
      io.println("Part 2: " <> { calculate(data2) |> int.to_string })
    }
    _ -> io.println("Usage: gleam run -- <input-file>")
  }
}

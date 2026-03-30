import argv
import gleam/int
import gleam/io
import gleam/list
import gleam/string
import simplifile
import splitter

type Data =
  List(#(Int, Int))

fn parse_pass(to_parse, data: Data) {
  let pattern_start = splitter.new(["mul("])
  // Search for the start pattern
  let start_result = splitter.split_after(pattern_start, to_parse)
  case start_result {
    #(_, "") -> data
    // No more matches, return the data
    #(_, candidate) -> {
      let pattern_end = splitter.new([")"])
      // We got a potential candidate, we have to search for a matching end pattern
      let end_result = splitter.split_before(pattern_end, candidate)
      case end_result {
        #(_, "") -> data
        // No more matches, return the data
        #(param_str, _) -> {
          // Used to start the next pass
          let check_next = parse_pass(candidate, _)

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

pub fn parse(input: String) -> Data {
  string.trim(input)
  |> parse_pass([])
}

pub fn part1(data: Data) {
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
      let data = parse(input)
      io.println("Part 1: " <> { part1(data) |> int.to_string })
    }
    _ -> io.println("Usage: gleam run -- <input-file>")
  }
}

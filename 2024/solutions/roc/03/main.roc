app [main!] { pf: platform "https://github.com/roc-lang/basic-cli/releases/download/0.19.0/Hj-J_zxz7V9YurCSTFcFdu6cQJie4guzsPMUi5kBYUk.tar.br" }

import pf.Arg
import pf.File
import pf.Stdout

Data : Str

parse1 : Data -> _
parse1 = |data|
    Str.split_on(data, "mul(")
    |> List.drop_first(1) # First part is never part of a valid pattern
    |> List.keep_oks(
        |line|
            param_str_result =
                Str.split_on(line, ")")
                |> List.get(0)
            when param_str_result is
                Ok(param_str) ->
                    params =
                        param_str
                        |> Str.split_on(",")
                        |> List.map(Str.to_u32)
                    when params is
                        [Ok(param1), Ok(param2)] -> Ok((param1, param2))
                        _ -> Err({})

                Err(_) -> Err({}),
    )

expect parse1("mul(1,2)mul(3,4)") == [(1, 2), (3, 4)]
expect parse1("mul( 1,2 )mul(3,4)mul(#5,6+)") == [(3, 4)]

part1 : Data -> _
part1 = |data|
    parse1(data)
    |> List.map(|(param1, param2)| param1 * param2)
    |> List.sum

run! = |args|
    file = try(List.get, args, 1)
    input = try(File.read_utf8!, file)
    try(Stdout.line!, "Part1: ${part1(input) |> Num.to_str}")
    # try(Stdout.line!, "Part2: ${part2(input) |> Num.to_str}")
    Ok({})

main! = |raw_args|
    args = List.map(raw_args, Arg.display)
    run!(args)

app [main!] { pf: platform "https://github.com/roc-lang/basic-cli/releases/download/0.19.0/Hj-J_zxz7V9YurCSTFcFdu6cQJie4guzsPMUi5kBYUk.tar.br" }

import pf.Arg
import pf.File
import pf.Stdout

NumType : U32
Data : (List NumType, List NumType)

parse : Str -> Data
parse = |input|
    to_tuple : Str -> (NumType, NumType)
    to_tuple = |line|
        parts = Str.split_on(line, "   ")
        when parts is
            [x, y] ->
                x_parsed = Str.to_u32(x)
                y_parsed = Str.to_u32(y)
                when (x_parsed, y_parsed) is
                    (Ok(x_value), Ok(y_value)) -> (x_value, y_value)
                    _ -> crash("Failed to parse line")

            _ -> crash("Something went wrong parsing the input")

    Str.split_on(input, "\n")
    |> List.keep_if(|line| line != "")
    |> List.map(to_tuple)
    # Transform list of pairs to two lists
    |> List.walk(
        ([], []),
        |(list1, list2), (el1, el2)|
            (
                List.append(list1, el1),
                List.append(list2, el2),
            ),
    )

expect parse("1   2\n3   4\n5   6\n") == ([1, 3, 5], [2, 4, 6])

part1 : Data -> _
part1 = |(list1, list2)|
    # Sort lists
    list1_sorted = List.sort_asc(list1)
    list2_sorted = List.sort_asc(list2)
    List.map2(list1_sorted, list2_sorted, Num.abs_diff)
    |> List.sum

part2 : Data -> _
part2 = |(list1, list2)|
    List.map(
        list1,
        |x|
            occurances = List.count_if(list2, |y| x == y) |> Num.to_u32
            x * occurances,
    )
    |> List.sum

run! = |args|
    file = try(List.get, args, 1)
    input = try(File.read_utf8!, file)
    data = parse(input)
    try(Stdout.line!, "Part1: ${part1(data) |> Num.to_str}")
    try(Stdout.line!, "Part2: ${part2(data) |> Num.to_str}")
    Ok({})

main! = |raw_args|
    args = List.map(raw_args, Arg.display)
    run!(args)

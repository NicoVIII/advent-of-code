module [get_output_file_name]

import pf.Dir
import pf.File
import pf.Path
import ListExt

get_output_file_name_internal : Str -> Str
get_output_file_name_internal = \input_file ->
    prefix = Str.dropSuffix input_file "-input.txt"
    "$(prefix)-output.txt"

expect get_output_file_name_internal "../../../puzzles/01/example-input.txt" == "../../../puzzles/01/example-output.txt"

get_output_file_name : Path.Path -> Path.Path
get_output_file_name = \input_file ->
    Path.display input_file
    |> get_output_file_name_internal
    |> Path.from_str

fnc! : Str => _
fnc! = \day ->
    input_file_list = try Dir.list! "../../../puzzles/inputs/$(day)"
    # Create pairs of input and output files
    file_pair_list =
        ListExt.map!
            input_file_list
            (\input_file -> (input_file, get_output_file_name input_file))
    # Read files
    file_content_pair_list =
        ListExt.map!
            file_pair_list
            (\(input_file, output_file) ->
                input_content = try Path.read_utf8! input_file
                output_content = try Path.read_utf8! output_file
                Ok (input_content, output_content)
            )
    Ok {}

# get_expected_list = \(dayDir, part1, part2) ->
#     input_file_list = try Dir.list! "../../../puzzles/inputs/01"
#     expected_list =
#         List.walk!
#             input_file_list
#             []
#             (\input_file ->
#                 expected_file =
#                     { before, after } =
#                         try file
#                         |> Path.display
#                         |> Str.splitLast "/"
#                     key =
#                         Str.dropPrefix after "input-"
#                         |> Str.dropSuffix ".txt"
#                     before + "output-" + key + ".txt"
#                 try File.read_utf8! expected_file
#             )
#     actual_list = List.map input_file_list (\input_file -> run! [input_file])
#     actual_list == expected_list

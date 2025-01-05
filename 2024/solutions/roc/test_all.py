#!/usr/bin/env python3
"""This script runs the roc run command for each input file in the folders
and compares the output to the expected output in the corresponding output file."""

import os
import subprocess
import sys
from typing import Final


def run_test_for_file(folder: str, input_path: str) -> list[str] | None:
    """This function runs a test for a given input file"""
    output_path: Final = input_path.replace('-input.txt', '-output.txt')

    # Read out two lines from the output file into variables
    with open(output_path, 'r', encoding="utf8") as f:
        lines = f.readlines()
        # Assuming the first two lines are the ones we want
        expected_output1 = lines[0].strip()
        expected_output2 = lines[1].strip()

    # Call roc run with the input file and compare the output to the expected output
    # We have to add an additional .. because we run roc in the subfolders
    result = subprocess.run(
        ['roc', 'dev', '--', f'../{input_path}'],
        capture_output=True,
        text=True,
        check=False,
        cwd=folder
    )
    if result.returncode != 0:
        return [f'Error: {result.returncode} {result.stdout} {result.stderr}']
    output_lines = result.stdout.splitlines()
    if len(output_lines) > 2 or len(output_lines) < 1:
        return ['Error: Output does not match expected output']
    output_line1 = output_lines[0].replace('Part1: ', '').strip()

    # Compare the output to the expected output
    errors: list[str] = []
    if output_line1 != expected_output1:
        errors.append(
            'Error: Output line 1 does not match expected output: ' +
            f'{output_line1} != {expected_output1}'
        )

    # Check if the second line is present
    if len(output_lines) == 2 and expected_output2 != '-':
        output_line2 = output_lines[1].replace('Part2: ', '').strip()
        if output_line2 != expected_output2:
            errors.append(
                'Error: Output line 2 does not match expected output: ' +
                f'{output_line2} != {expected_output2}'
            )

    return errors if len(errors) > 0 else None


def run_tests_for_folder(folder: str) -> bool:
    """This function runs tests for a given folder"""
    successful_runs = 0

    input_paths: Final = [
        os.path.join(f'../../puzzles/{folder}', f) for f in os.listdir(f'../../puzzles/{folder}') if f.endswith('-input.txt')
    ]
    # Get the number of input files
    num_inputs: Final = len(input_paths)
    for input_path in input_paths:
        result = run_test_for_file(folder, input_path)
        if result is not None:
            print(f'! Errors in {input_path}:')
            for error in result:
                print(f'- {error}')
        else:
            successful_runs += 1

    # Print the results
    print(
        f'Finished running tests in {folder}: {successful_runs}/{num_inputs} successful'
    )

    return successful_runs == num_inputs


def main() -> None:
    """Main function"""
    success = True
    folders: Final = [
        f for f in os.listdir('.') if os.path.isdir(f) and len(f) == 2
    ]
    # Sort the folders by their name
    folders.sort()
    for folder in folders:
        success = run_tests_for_folder(folder) and success

    if not success:
        print('Some tests failed')
        sys.exit(1)
    else:
        print('All tests passed')
        sys.exit(0)


if __name__ == '__main__':
    main()

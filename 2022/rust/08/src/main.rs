use std::{fs, str::Lines};

fn read_input() -> String {
    fs::read_to_string("input.txt").expect("Should have been able to read the file")
}

type TreeRectangleRows = Vec<Vec<u8>>;

#[derive(Debug)]
struct TreeRectangle {
    horizontal: TreeRectangleRows,
    vertical: TreeRectangleRows,
}

fn parse_input(lines: Lines<'_>) -> TreeRectangleRows {
    lines
        .filter(|line| !line.is_empty())
        .map(|line| {
            line.chars()
                .map(|c| c.to_string().parse::<u8>().unwrap())
                .collect()
        })
        .collect()
}

fn precalc_rectangle(rows: TreeRectangleRows) -> TreeRectangle {
    let mut columns: TreeRectangleRows = vec![];
    for _ in 0..rows[0].len() {
        columns.push(vec![]);
    }
    // Fill column vectors
    for row in &rows {
        for (i, entry) in row.iter().enumerate() {
            columns[i].push(*entry);
        }
    }
    TreeRectangle {
        horizontal: rows,
        vertical: columns,
    }
}

fn count_visible_trees(rectangle: &TreeRectangle) -> u32 {
    fn is_tree_visible(rectangle: &TreeRectangle, x: usize, y: usize) -> bool {
        let value = rectangle.horizontal[y][x];
        let from_left = rectangle.horizontal[y][..x].iter().all(|&x| x < value);
        let from_right = rectangle.horizontal[y][x + 1..].iter().all(|&x| x < value);
        let from_top = rectangle.vertical[x][..y].iter().all(|&x| x < value);
        let from_bottom = rectangle.vertical[x][y + 1..].iter().all(|&x| x < value);
        from_left || from_right || from_top || from_bottom
    }
    let mut visible = 0;
    for y in 0..rectangle.horizontal.len() {
        for x in 0..rectangle.horizontal[y].len() {
            if is_tree_visible(rectangle, x, y) {
                visible += 1;
            }
        }
    }
    visible
}

fn get_max_scenic_score(rectangle: &TreeRectangle) -> usize {
    fn get_scenic_score(rectangle: &TreeRectangle, x: usize, y: usize) -> usize {
        let value = rectangle.horizontal[y][x];
        let score_left = {
            let blocker = rectangle.horizontal[y][..x]
                .iter()
                .rposition(|&v| v >= value)
                .unwrap_or(0);
            x - blocker
        };
        let score_right = {
            let blocker = rectangle.horizontal[y][x + 1..]
                .iter()
                .position(|&v| v >= value)
                .map(|v| v + 1)
                .unwrap_or_else(|| rectangle.horizontal[y][x + 1..].len());
            blocker
        };
        let score_top = {
            let blocker = rectangle.vertical[x][..y]
                .iter()
                .rposition(|&v| v >= value)
                .unwrap_or(0);
            y - blocker
        };
        let score_bottom = {
            let blocker = rectangle.vertical[x][y + 1..]
                .iter()
                .position(|&v| v >= value)
                .map(|v| v + 1)
                .unwrap_or_else(|| rectangle.vertical[x][y + 1..].len());
            blocker
        };
        score_left * score_right * score_top * score_bottom
    }

    let mut max_score = 0;
    for y in 0..rectangle.horizontal.len() {
        for x in 0..rectangle.horizontal[y].len() {
            let score = get_scenic_score(rectangle, x, y);
            if score > max_score {
                max_score = score;
            }
        }
    }
    max_score
}

fn main() {
    let input = read_input();
    let tree_rectangle = precalc_rectangle(parse_input(input.lines()));

    // Part 1
    let visible_trees = count_visible_trees(&tree_rectangle);
    println!("Part 1: {}", visible_trees);

    // Part 2
    let max_scenic_score = get_max_scenic_score(&tree_rectangle);
    println!("Part 2: {}", max_scenic_score);
}

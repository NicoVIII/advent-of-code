#!/usr/bin/env python3
"""
Helper script to convert Advent of Code puzzle HTML to Markdown format.
Usage: python parse-puzzle.py <input_html_file> [output_markdown_file]
"""

import sys
import re
from pathlib import Path

try:
    from bs4 import BeautifulSoup
except ImportError:
    print("Error: BeautifulSoup4 is required. Install it with: pip install beautifulsoup4")
    sys.exit(1)

def html_to_markdown(html_content: str) -> str:
    """Convert HTML content to Markdown."""
    soup = BeautifulSoup(html_content, 'html.parser')
    
    markdown = ""
    
    for element in soup.children:
        if isinstance(element, str):
            text = element.strip()
            if text:
                markdown += text + "\n"
        elif element.name == 'h1':
            markdown += f"# {element.get_text()}\n\n"
        elif element.name == 'h2':
            markdown += f"## {element.get_text()}\n\n"
        elif element.name == 'h3':
            markdown += f"### {element.get_text()}\n\n"
        elif element.name == 'h4':
            markdown += f"#### {element.get_text()}\n\n"
        elif element.name == 'h5':
            markdown += f"##### {element.get_text()}\n\n"
        elif element.name == 'h6':
            markdown += f"###### {element.get_text()}\n\n"
        elif element.name == 'p':
            markdown += _process_paragraph(element) + "\n\n"
        elif element.name == 'pre':
            markdown += _process_pre(element) + "\n\n"
        elif element.name == 'ul':
            markdown += _process_list(element) + "\n\n"
        elif element.name == 'ol':
            markdown += _process_ordered_list(element) + "\n\n"
        elif element.name == 'blockquote':
            markdown += _process_blockquote(element) + "\n\n"
        elif element.name in ['div', 'article', 'section']:
            # Recursively process container elements
            markdown += html_to_markdown(str(element))
    
    # Clean up excess whitespace
    markdown = re.sub(r'\n\n\n+', '\n\n', markdown)
    return markdown.strip()

def _process_inline(element) -> str:
    """Process inline elements within a parent."""
    result = ""
    for child in element.children:
        if isinstance(child, str):
            result += child
        elif child.name == 'i':
            result += f"*{child.get_text()}*"
        elif child.name == 'em' or child.name == 'strong' or child.name == 'b':
            result += f"**{child.get_text()}**"
        elif child.name == 'code':
            result += f"`{child.get_text()}`"
        elif child.name == 'a':
            href = child.get('href', '#')
            text = child.get_text()
            result += f"[{text}]({href})"
        else:
            result += _process_inline(child)
    return result

def _process_paragraph(element) -> str:
    """Process paragraph element."""
    return _process_inline(element)

def _process_pre(element) -> str:
    """Process pre/code block element."""
    code_block = element.get_text()
    # Remove trailing/leading whitespace per line
    lines = code_block.split('\n')
    # Find minimum indentation
    min_indent = float('inf')
    for line in lines:
        if line.strip():
            indent = len(line) - len(line.lstrip())
            min_indent = min(min_indent, indent)
    
    if min_indent == float('inf'):
        min_indent = 0
    
    # Remove common indentation
    dedented_lines = [line[min_indent:] if len(line) > min_indent else line 
                      for line in lines]
    code_block = '\n'.join(dedented_lines).strip()
    
    return f"```\n{code_block}\n```"

def _process_list(element) -> str:
    """Process unordered list element."""
    result = ""
    for item in element.find_all('li', recursive=False):
        result += f"- {_process_inline(item)}\n"
    return result.rstrip()

def _process_ordered_list(element) -> str:
    """Process ordered list element."""
    result = ""
    for i, item in enumerate(element.find_all('li', recursive=False), 1):
        result += f"{i}. {_process_inline(item)}\n"
    return result.rstrip()

def _process_blockquote(element) -> str:
    """Process blockquote element."""
    text = _process_inline(element)
    lines = text.split('\n')
    return '\n'.join(f"> {line}" for line in lines)

def main():
    if len(sys.argv) < 2:
        print("Usage: python parse-puzzle.py <input_html_file> [output_markdown_file]")
        print("\nIf output file is not specified, it will be saved as 'puzzle.md'")
        sys.exit(1)
    
    input_file = Path(sys.argv[1])
    output_file = Path(sys.argv[2]) if len(sys.argv) > 2 else Path("puzzle.md")
    
    # Read HTML file
    if not input_file.exists():
        print(f"Error: Input file '{input_file}' not found")
        sys.exit(1)
    
    try:
        html_content = input_file.read_text(encoding='utf-8')
    except Exception as e:
        print(f"Error reading input file: {e}")
        sys.exit(1)
    
    # Convert to Markdown
    markdown_content = html_to_markdown(html_content)
    
    # Save to output file
    try:
        output_file.write_text(markdown_content, encoding='utf-8')
        print(f"✓ Converted HTML to Markdown")
        print(f"✓ Saved to: {output_file.absolute()}")
    except Exception as e:
        print(f"Error writing output file: {e}")
        sys.exit(1)

if __name__ == "__main__":
    main()

global args
import argparse
import os
parser = argparse.ArgumentParser(description='text')
parser.add_argument('name', help='name')
parser.add_argument('num', help='dialogue numbers')
args = parser.parse_args()

name = args.name.strip()
numbers = list(args.num.strip().split(" "))
for numb in numbers:
    file_object  = open(numb + name + ".txt", "w")
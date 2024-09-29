# refactor from nested calls to method chains

### business request #1
1. 30% discount on martin fowler patterns books
2. or then 20% on books of patterns
3. or flat 10% all books RelativeDiscount(0.1M)

nested init:
DemoDiscountServerNested

chained final:
DemoDiscountServer
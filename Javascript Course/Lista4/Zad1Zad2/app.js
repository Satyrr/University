// Zad1

var Tree = function(leftTree, rightTree, val)
{
    this.leftTree = leftTree;
    this.rightTree = rightTree;
    this.val = val;
    this[Symbol.iterator] = treeEnumerator;
}

tree = new Tree(
    null, new Tree(new Tree(new Tree(null,null,1), new Tree(null,null,6), 2),null,3), 5)

console.log(tree.val)
console.log(tree.rightTree.val)
console.log(tree.rightTree.leftTree.val)
console.log(tree.rightTree.leftTree.rightTree.val)

//Zad2

tree2 = new Tree(new Tree(null,null,1), new Tree(null,null,3), 2)
function* treeEnumerator()
{
    if(this.leftTree)
        yield* this.leftTree
        
    yield this.val  

    if(this.rightTree)
        yield* this.rightTree
    
}

console.log("for of loop:")
for(v of tree2)
    console.log(v)

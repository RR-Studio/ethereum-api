"Full" Sync: Gets the block headers, the block bodies, and validate every element from genesis block.
Fast Sync: Gets the block headers, the block bodies, it performs no validation until current block - 1024. Then it gets a snap shot state, and goes like a full synchronisation.
Light Sync: Gets only the current state. To verify elements, needs to ask to full (archive) nodes for the corresponding tree leave.


��� �������� ����������
https://medium.com/@codetractio/inside-an-ethereum-transaction-fa94ffca912f
������������� ����������
https://ethereum.stackexchange.com/questions/6002/transaction-status
�������
https://ethereum.stackexchange.com/questions/2531/common-useful-javascript-snippets-for-geth/3478#3478
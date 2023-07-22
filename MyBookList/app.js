//Book Class: Represent a book
class Book {
    constructor(title, author, isbn) {
        this.title = title;
        this.author = author;
        this.isbn = isbn;
    }
}
//UI Class: Handle UI Task
class UI {
    static displayBooks() {
        
        //Hardcoded Books
        // const StoredBooks = [
        //     {
        //         title: 'Book1',
        //         author: 'Khaja',
        //         isbn: '1234'
        //     },
        //     {
        //         title: 'Book2',
        //         author: 'Mohideen',
        //         isbn: '5678'
        //     }
        // ];

        // const books = StoredBooks;

        const books=Store.getBooks();

        books.forEach((book) => UI.addBooksToList(book));
    }

    static addBooksToList(book) {
        const list = document.querySelector('#book-list');

        const row = document.createElement('tr');

        row.innerHTML = `
        <td>${book.title}</td>
        <td>${book.author}</td>
        <td>${book.isbn}</td>
        <td><a href="#" class="btn btn-danger btn-sm delete">X</a></td>`;

        list.appendChild(row);

        Store.addBook(book);
    }


    static deleteBook(el){
        if(el.classList.contains('delete')){
            el.parentElement.parentElement.remove();
            
           Store.removeBook(el.parentElement.previousElementSibling.textContent);
            //Show success alert
            UI.showAlert('Deleted successfully','info');
        }
    }

    static showAlert(message,className)
    {
        const div=document.createElement('div');
        div.className=`alert alert-${className}`;
        div.appendChild(document.createTextNode(message));

        const container=document.querySelector('.container');
        const form=document.querySelector('#book-form');
        container.insertBefore(div,form);

        //Remove alert after 2 seconds
        setTimeout(() => {
            document.querySelector('.alert').remove();
        }, 2000);
    }

    static clearFields(){
        document.querySelector('#title').value='';
        document.querySelector('#author').value='';
        document.querySelector('#isbn').value='';

    }
}

//Store Class: Handle Storages
class Store{
    static getBooks(){
        let books;
        if(localStorage.getItem('books')===null){
            books=[];
        }else{
            books=JSON.parse(localStorage.getItem('books'));
        }

        return books;
    }

    static addBook(book){
        let books=Store.getBooks()
        books.push(book);
        localStorage.setItem('books',JSON.stringify(books));
    }

    static removeBook(isbn){
        let books=Store.getBooks();

        books.forEach((book,index)=>{
            if(book.isbn===isbn){
                books.splice(index,1);
            }
        });
        
        localStorage.setItem('books',JSON.stringify(books));

    }
}

//Events : Display Books
document.addEventListener('DOMContentLoaded', UI.displayBooks)

//Events : Add a Book
document.querySelector('#book-form').addEventListener('submit', (e) => {
    //Prevent actual submit
    e.preventDefault();

    //Get Form Values
    const title = document.querySelector('#title').value;
    const author = document.querySelector('#author').value;
    const isbn = document.querySelector('#isbn').value;

    //Validate
    if(title==='' || author==='' || isbn===''){
        UI.showAlert('Please fill all the fields!','danger')
    }else{
        //Instantiate a Book
        const book = new Book(title, author, isbn);

        //Add Book to UI
        UI.addBooksToList(book);

        //Show success alert
        UI.showAlert('Added successfully','success');

        //Clear Fields
        UI.clearFields();
    }

    
});


//Events : Remove a Book
document.querySelector('#book-list').addEventListener('click',(e)=>{
    UI.deleteBook(e.target);

});
from flask import Flask, jsonify, request
import mysql.connector


app = Flask(__name__)

# Configurações do banco de dados MySQL
db_config = {
    'user': 'adm',
    'password': 'A8108@76a',
    'host': 'localhost',
    'database': 'ajenick',
}

# Rota para buscar os itens
@app.route('/get_items', methods=['GET'])
def get_items():
    conn = mysql.connector.connect(**db_config)
    cursor = conn.cursor(dictionary=True)
    cursor.execute("SELECT nome, descricao, id_jogo FROM jogo")
    rows = cursor.fetchall()
    cursor.close()
    conn.close()
    return jsonify(rows)

# Rota para buscar perguntas com base no id_jogo
@app.route('/get_questions', methods=['GET'])
def get_questions():
    id_jogo = request.args.get('id_jogo')
    if not id_jogo:
        return jsonify({'error': 'id_jogo não fornecido'}), 400
    
    conn = mysql.connector.connect(**db_config)
    cursor = conn.cursor(dictionary=True)
    cursor.execute("SELECT * FROM pergunta WHERE id_jogo = %s", (id_jogo,))
    rows = cursor.fetchall()
    cursor.close()
    conn.close()
    return jsonify(rows)

@app.route('/add_game', methods=['POST'])
def add_game():
    data = request.get_json()
    nome = data['Titulo']
    descricao = data['Descricao']
    
    conn = mysql.connector.connect(**db_config)
    cursor = conn.cursor()
    cursor.execute("INSERT INTO jogo (nome, descricao) VALUES (%s, %s)", (nome, descricao))
    conn.commit()
    id_jogo = cursor.lastrowid
    cursor.close()
    conn.close()
    return jsonify(id_jogo)

@app.route('/add_questions', methods=['POST'])
def add_questions():
    data = request.get_json()
    perguntas = data['perguntas']
    
    conn = mysql.connector.connect(**db_config)
    cursor = conn.cursor()
    
    for pergunta in perguntas:
        cursor.execute("INSERT INTO pergunta (questao, alternativa1, alternativa2, alternativa3, alternativa4, resposta, id_jogo) VALUES (%s, %s, %s, %s, %s, %s, %s)",
                       (pergunta['questao'], pergunta['alternativa1'], pergunta['alternativa2'], pergunta['alternativa3'], pergunta['alternativa4'], pergunta['indexRes'], pergunta['id_jogo']))
    
    conn.commit()
    cursor.close()
    conn.close()
    return jsonify({'status': 'success'})

if __name__ == '__main__':
    app.run(host='127.0.0.1', port=5000, debug=True)

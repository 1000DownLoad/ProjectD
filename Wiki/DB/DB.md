# DB
파이어스토어 DB 사용 방법에 대하여 정리합니다.

파이어스토어 DB에서 사용되는 용어
Collection : 기본적으로 우리가 알고 있는 테이블에 해당
Document : Collection 내에 있는 하나의 문서
Field : 문서 내에 있는 항목들

파이어스토어에서 일반적으로 사용되는 데이터형 : Dictionary<string, object> 

# 세부 동작
DataBaseManager에서 파이어스토어 DB를 관리합니다.
- Initialize : SDK에서 생성된 서비스 계정 키(JSON 파일)의 경로를 지정하고 파이어스토어 SDK에 액세스를 합니다.
- m_firestore_DB : 파이어스토어 DB 객체
- UpdateDataBase(collection_name, doc_name, data) : 해당 문서를 찾아서 데이터 업데이트(데이터가 없을 경우 추가)
- GetCollectionData(collection_name) : 해당 컬렉션의 문서들을 가져옵니다.
- GetDocumentData(collection_name, doc_name) : 해당 컬렉션의 문서를 가져옵니다.
- ReadDocumentsByCondition : 조건이 일치하는 문서들을 읽습니다.




